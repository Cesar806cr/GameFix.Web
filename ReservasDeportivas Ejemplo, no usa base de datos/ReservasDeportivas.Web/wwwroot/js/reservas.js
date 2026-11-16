const $ = id => document.getElementById(id);

const actividadId = $("actividadId");
const actividadNombre = $("actividadNombre");
const fechaActividad = $("fechaActividad");
const horaInicio = $("horaInicio");
const formReserva = $("formReserva");

let calendario;

const buscarActividad = $("buscarActividad");
const sugerencias = $("sugerenciasActividad");
let temporizadorBusqueda;

buscarActividad.addEventListener("input", () => {
    clearTimeout(temporizadorBusqueda);

    temporizadorBusqueda = setTimeout(async () => {
        const termino = buscarActividad.value.trim();

        sugerencias.replaceChildren();

        if (termino.length < 2) return;

        try {
            const respuesta = await fetch(
                `/Reservas/BuscarActividades?termino=${encodeURIComponent(termino)}`
            );

            if (!respuesta.ok) return;

            const actividades = await respuesta.json();

            actividades.forEach(actividad => {
                const opcion = document.createElement("button");

                opcion.type = "button";
                opcion.className = "list-group-item list-group-item-action";
                opcion.textContent = actividad.nombre;

                opcion.addEventListener("click", () => {
                    const tarjeta = document.querySelector(
                        `.actividad-card[data-id="${actividad.id}"]`
                    );

                    tarjeta?.querySelector(
                        ".seleccionar-actividad"
                    ).click();

                    buscarActividad.value = actividad.nombre;
                    sugerencias.replaceChildren();
                });

                sugerencias.appendChild(opcion);
            });
        } catch {
            mostrarToast("No fue posible buscar las actividades.");
        }
    }, 250);
});

document.addEventListener("click", event => {
    if (!buscarActividad.contains(event.target) &&
        !sugerencias.contains(event.target)) {
        sugerencias.replaceChildren();
    }
});


function mostrarToast(mensaje) {
    $("toastMensaje").textContent = mensaje;

    bootstrap.Toast.getOrCreateInstance(
        $("appToast")
    ).show();
}

async function cargarHorarios() {
    const id = actividadId.value;
    const fecha = fechaActividad.value;

    horaInicio.innerHTML =
        '<option value="">Cargando horarios...</option>';

    if (!id || !fecha) {
        horaInicio.innerHTML =
            '<option value="">Seleccione actividad y fecha</option>';
        return;
    }

    try {
        const url = `/Reservas/Horarios?actividadId=${encodeURIComponent(id)}&fecha=${encodeURIComponent(fecha)}`;

        const respuesta = await fetch(url);

        if (!respuesta.ok) {
            throw new Error("No fue posible consultar los horarios.");
        }

        const horarios = await respuesta.json();

        horaInicio.innerHTML = "";

        if (horarios.length === 0) {
            horaInicio.add(new Option(
                "No hay horarios disponibles", ""));
            return;
        }

        horaInicio.add(new Option("Seleccione una hora", ""));

        horarios.forEach(hora => {
            horaInicio.add(new Option(hora, hora));
        });
    } catch (error) {
        horaInicio.innerHTML =
            '<option value="">Error al consultar horarios</option>';

        mostrarToast(error.message);
    }
}

document.querySelectorAll(".actividad-card").forEach(card => {
    const seleccionar = () => {
        actividadId.value = card.dataset.id;
        actividadNombre.value =
            card.querySelector(".card-title").textContent.trim();

        document.querySelectorAll(".actividad-card")
            .forEach(c => c.classList.remove("border-primary", "border-3"));

        card.classList.add("border-primary", "border-3");

        cargarHorarios();
    };

    card.querySelector(".seleccionar-actividad")
        .addEventListener("click", seleccionar);

    card.addEventListener("keydown", event => {
        if (event.key === "Enter" || event.key === " ") {
            event.preventDefault();
            seleccionar();
        }
    });
});

fechaActividad.min = new Date().toLocaleDateString("en-CA");
fechaActividad.addEventListener("change", cargarHorarios);

formReserva.addEventListener("submit", async event => {
    event.preventDefault();

    if (!formReserva.reportValidity()) return;

    if (!actividadId.value || !horaInicio.value) {
        mostrarToast("Seleccione una actividad y un horario.");
        return;
    }

    const confirmacion = await Swal.fire({
        title: "¿Confirmar reservación?",
        text: `${actividadNombre.value} a las ${horaInicio.value}`,
        icon: "question",
        showCancelButton: true,
        confirmButtonText: "Sí, reservar",
        cancelButtonText: "Cancelar"
    });

    if (!confirmacion.isConfirmed) return;

    const token = formReserva.querySelector(
        'input[name="__RequestVerificationToken"]'
    ).value;

    const dto = {
        actividadId: Number(actividadId.value),
        nombreCliente: $("nombreCliente").value.trim(),
        correo: $("correo").value.trim(),
        telefono: $("telefono").value.trim(),
        fechaActividad: fechaActividad.value,
        horaInicio: horaInicio.value
    };

    try {
        const respuesta = await fetch("/Reservas/Crear", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "RequestVerificationToken": token
            },
            body: JSON.stringify(dto)
        });

        const resultado = await respuesta.json();

        if (!respuesta.ok) {
            const detalle = resultado.errores?.join("\n");
            throw new Error(detalle || resultado.mensaje ||
                "No se pudo guardar la reservación.");
        }

        await Swal.fire({
            title: "¡Reservación creada!",
            text: resultado.mensaje,
            icon: "success",
            confirmButtonText: "Aceptar"
        });

        formReserva.reset();
        actividadId.value = "";
        actividadNombre.value = "";

        document.querySelectorAll(".actividad-card")
            .forEach(c => c.classList.remove("border-primary", "border-3"));

        horaInicio.innerHTML =
            '<option value="">Seleccione actividad y fecha</option>';

        calendario.refetchEvents();
        mostrarToast("Reservación guardada correctamente.");
    } catch (error) {
        Swal.fire({
            title: "No se pudo reservar",
            text: error.message,
            icon: "error"
        });

        cargarHorarios();
    }
});

document.addEventListener("DOMContentLoaded", () => {
    calendario = new FullCalendar.Calendar($("calendario"), {
        initialView: "timeGridWeek", //CAMBIAR A dayGridMonth para verlo por mes, timeGridDay verlo por dia verlo por semana timeGridWeek
        locale: "es",
        height: 650,
        headerToolbar: {
            left: "prev,next today",
            center: "title",
            right: "dayGridMonth,timeGridWeek,timeGridDay"
        },
        events: "/Reservas/Agenda",
        eventClick: info => mostrarDetalle(info.event)
    });

    calendario.render();
});

async function mostrarDetalle(evento) {
    const props = evento.extendedProps;

    const opciones = await Swal.fire({
        title: evento.title,
        html: `
            <p><b>Inicio:</b> ${evento.start.toLocaleString("es-CR")}</p>
            <p><b>Fin:</b> ${evento.end?.toLocaleString("es-CR") ?? ""}</p>
            <p><b>Teléfono:</b> ${props.telefono}</p>
            <p><b>Correo:</b> ${props.correo}</p>
        `,
        showCancelButton: true,
        showDenyButton: true,
        confirmButtonText: "WhatsApp",
        denyButtonText: "Eliminar",
        cancelButtonText: "Cerrar",
        icon: "info"
    });

    if (opciones.isConfirmed) {
        const texto = `Recordatorio de reservación\n` +
            `Cliente: ${props.cliente}\n` +
            `Actividad: ${evento.title}\n` +
            `Fecha: ${evento.start.toLocaleDateString("es-CR")}\n` +
            `Hora: ${evento.start.toLocaleTimeString("es-CR", {
                hour: "2-digit", minute: "2-digit"
            })}`;

        const telefono = props.telefono.replace(/\D/g, "");

        window.open(
            `https://api.whatsapp.com/send?phone=${telefono}&text=${encodeURIComponent(texto)}`,
            "_blank",
            "noopener,noreferrer"
        );
    }

    if (opciones.isDenied) {
        const confirmacion = await Swal.fire({
            title: "¿Eliminar reservación?",
            text: "Esta operación no se puede deshacer.",
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: "Eliminar",
            cancelButtonText: "Cancelar"
        });

        if (!confirmacion.isConfirmed) return;

        const token = document.querySelector(
            'input[name="__RequestVerificationToken"]'
        ).value;

        const respuesta = await fetch(
            `/Reservas/Eliminar?id=${encodeURIComponent(evento.id)}`,
            {
                method: "DELETE",
                headers: {
                    "RequestVerificationToken": token
                }
            }
        );

        if (respuesta.ok) {
            calendario.refetchEvents();
            mostrarToast("Reservación eliminada.");
        } else {
            mostrarToast("No se pudo eliminar la reservación.");
        }
    }
}
