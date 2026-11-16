
"use strict";

let agendaAdmin = null;

document.addEventListener("DOMContentLoaded", () => {
    inicializarAgenda();
    configurarDesactivacion();
});

function inicializarAgenda() {
    const contenedor = document.getElementById("agendaAdmin");

    if (!contenedor) return;

    if (typeof FullCalendar === "undefined") {
        mostrarError("FullCalendar no se ha cargado. Revisa _Layout.cshtml.");
        return;
    }

    const filtro = document.getElementById("filtroActividad");

    agendaAdmin = new FullCalendar.Calendar(contenedor, {
        initialView: "timeGridDay",
        locale: "es",
        height: 650,
        nowIndicator: true,
        slotDuration: "01:00:00",
        snapDuration: "01:00:00",
        allDaySlot: false,

        headerToolbar: {
            left: "prev,next today",
            center: "title",
            right: "timeGridDay,timeGridWeek,dayGridMonth"
        },

        buttonText: {
            today: "Hoy",
            day: "Día",
            week: "Semana",
            month: "Mes"
        },

        events: async (info, successCallback, failureCallback) => {
            try {
                const params = new URLSearchParams({
                    start: info.startStr,
                    end: info.endStr
                });

                if (filtro && filtro.value) {
                    params.set("actividadId", filtro.value);
                }

                const response = await fetch(
                    `/Admin/Agenda?${params.toString()}`,
                    {
                        method: "GET",
                        headers: { Accept: "application/json" }
                    }
                );

                const data = await leerRespuesta(response);

                if (!response.ok) {
                    throw new Error(
                        data.mensaje || `Error HTTP ${response.status}`
                    );
                }

                if (!Array.isArray(data)) {
                    throw new Error("La respuesta de la agenda no es una lista.");
                }

                successCallback(data);
            } catch (error) {
                console.error("Error al consultar la agenda:", error);
                failureCallback(error);
                mostrarToast(error.message, "danger");
            }
        },

        eventClick: async (info) => {
            info.jsEvent.preventDefault();

            const evento = info.event;
            const datos = evento.extendedProps || {};

            const fechaInicio = evento.start
                ? evento.start.toLocaleString("es-CR")
                : "No disponible";

            const fechaFin = evento.end
                ? evento.end.toLocaleString("es-CR")
                : "No disponible";

            const resultado = await Swal.fire({
                title: "Detalle de reservación",
                html: `
                    <div class="text-start">
                        <p><strong>Reservación:</strong> ${escaparHtml(evento.title)}</p>
                        <p><strong>Cliente:</strong> ${escaparHtml(datos.cliente || "No disponible")}</p>
                        <p><strong>Actividad:</strong> ${escaparHtml(datos.actividad || "No disponible")}</p>
                        <p><strong>Inicio:</strong> ${escaparHtml(fechaInicio)}</p>
                        <p><strong>Fin:</strong> ${escaparHtml(fechaFin)}</p>
                        <p><strong>Teléfono:</strong> ${escaparHtml(datos.telefono || "No registrado")}</p>
                        <p><strong>Estado:</strong> ${escaparHtml(datos.estado || "No disponible")}</p>
                    </div>
                `,
                icon: "info",
                showCancelButton: Boolean(datos.telefono),
                confirmButtonText: "Cerrar",
                cancelButtonText: "Preparar WhatsApp"
            });

            if (resultado.dismiss === Swal.DismissReason.cancel && datos.telefono) {
                abrirWhatsApp(datos.telefono, {
                    cliente: datos.cliente,
                    actividad: datos.actividad || evento.title,
                    fecha: evento.start
                });
            }
        }
    });

    agendaAdmin.render();

    document.getElementById("btnFiltrar")
        ?.addEventListener("click", actualizarAgenda);

    document.getElementById("btnActualizarAgenda")
        ?.addEventListener("click", actualizarAgenda);

    filtro?.addEventListener("change", actualizarAgenda);
}

function actualizarAgenda() {
    if (agendaAdmin) {
        agendaAdmin.refetchEvents();
    }
}

function configurarDesactivacion() {
    document.addEventListener("click", async (event) => {
        const boton = event.target.closest(".js-desactivar-actividad");

        if (!boton) return;

        const id = boton.dataset.id;
        const nombre = boton.dataset.nombre || "esta actividad";

        await eliminarActividad(id, nombre);
    });
}

async function eliminarActividad(id, nombre) {
    if (typeof Swal === "undefined") {
        alert("SweetAlert2 no se ha cargado.");
        return;
    }

    const confirmacion = await Swal.fire({
        title: "¿Desactivar actividad?",
        text: `La actividad "${nombre}" dejará de estar disponible para nuevas reservaciones.`,
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Sí, desactivar",
        cancelButtonText: "Cancelar",
        confirmButtonColor: "#dc3545"
    });

    if (!confirmacion.isConfirmed) return;

    try {
        const token = obtenerToken();

        const response = await fetch(
            `/Admin/EliminarActividad?id=${encodeURIComponent(id)}`,
            {
                method: "DELETE",
                headers: {
                    "Accept": "application/json",
                    "RequestVerificationToken": token
                }
            }
        );

        const data = await leerRespuesta(response);

        if (!response.ok) {
            throw new Error(
                data.mensaje || `No se pudo desactivar. HTTP ${response.status}`
            );
        }

        await Swal.fire(
            "Actividad desactivada",
            data.mensaje || "La actividad se desactivó correctamente.",
            "success"
        );

        window.location.reload();
    } catch (error) {
        console.error("Error al desactivar actividad:", error);
        mostrarError(error.message);
    }
}

function obtenerToken() {
    const input = document.querySelector(
        '#formToken input[name="__RequestVerificationToken"]'
    );

    if (!input) {
        throw new Error("No se encontró el token antifalsificación.");
    }

    return input.value;
}

async function leerRespuesta(response) {
    const contentType = response.headers.get("content-type") || "";

    if (contentType.includes("application/json")) {
        return await response.json();
    }

    const text = await response.text();

    return {
        mensaje: text || `Error HTTP ${response.status}`
    };
}

function abrirWhatsApp(telefono, datos = {}) {
    let numero = String(telefono || "").replace(/\D/g, "");

    if (!numero) {
        mostrarToast("No hay teléfono registrado.", "warning");
        return;
    }

    if (numero.length === 8) {
        numero = "506" + numero;
    }

    const fecha = datos.fecha
        ? datos.fecha.toLocaleString("es-CR")
        : "por confirmar";

    const mensaje =
        `Hola ${datos.cliente || ""}, te recordamos tu reservación deportiva.\n` +
        `Actividad: ${datos.actividad || "Actividad deportiva"}\n` +
        `Fecha y hora: ${fecha}\n¡Te esperamos!`;

    window.open(
        `https://wa.me/${numero}?text=${encodeURIComponent(mensaje)}`,
        "_blank",
        "noopener,noreferrer"
    );
}

function mostrarToast(mensaje, tipo = "success") {
    const elemento = document.getElementById("appToast");
    const cuerpo = document.getElementById("toastMensaje");

    if (!elemento || !cuerpo || !window.bootstrap) {
        console.log(mensaje);
        return;
    }

    elemento.classList.remove(
        "text-bg-success", "text-bg-danger",
        "text-bg-warning", "text-bg-info"
    );

    const clases = {
        success: "text-bg-success",
        danger: "text-bg-danger",
        warning: "text-bg-warning",
        info: "text-bg-info"
    };

    elemento.classList.add(clases[tipo] || clases.info);
    cuerpo.textContent = mensaje;

    bootstrap.Toast.getOrCreateInstance(elemento).show();
}

function mostrarError(mensaje) {
    if (typeof Swal !== "undefined") {
        Swal.fire("Error", mensaje, "error");
    } else {
        alert(mensaje);
    }
}

function escaparHtml(valor) {
    return String(valor ?? "").replace(/[&<>"']/g, caracter => ({
        "&": "&amp;",
        "<": "&lt;",
        ">": "&gt;",
        '"': "&quot;",
        "'": "&#039;"
    })[caracter]);
}
