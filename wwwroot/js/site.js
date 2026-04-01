// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", () => {
  const dataInput = document.querySelector(".reserva-data");
  const horarioInput = document.querySelector(".reserva-horario");

  if (!dataInput || !horarioInput) {
    return;
  }

  const hoje = new Date().toISOString().split("T")[0];
  const minimoHoje = horarioInput.dataset.minHoje || horarioInput.dataset.minPadrao;
  const minimoPadrao = horarioInput.dataset.minPadrao;
  const maximoHorario = horarioInput.dataset.maxHorario;

  dataInput.min = hoje;

  const atualizarRestricoesHorario = () => {
    if (dataInput.value === hoje) {
      horarioInput.min = minimoHoje;

      if (horarioInput.value && horarioInput.value < minimoHoje) {
        horarioInput.value = minimoHoje;
      }
    } else {
      horarioInput.min = minimoPadrao;
    }

    horarioInput.max = maximoHorario;
  };

  atualizarRestricoesHorario();
  dataInput.addEventListener("change", atualizarRestricoesHorario);
});
