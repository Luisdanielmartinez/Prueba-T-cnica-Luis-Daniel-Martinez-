<template>
    <div class="container my-3">
    <h2>Resultado de la Apuesta</h2>
    <p>Resultado: {{ result }}</p>
    <p>Ganancia: {{ prize }}</p>
    <button @click="saveGame" class="btn btn-primary">Guardar Juego</button>
    <button @click="exit" class="btn btn-red">Salir del juego</button>
  </div>
  </template>
  
  <script>
   import axios from 'axios';
  export default {
    props: ['result', 'prize', 'userName', 'amount'],
    methods: {
      async saveGame() {
        try {
          await axios.post('https://localhost:44351/api/roulette/save', {
          name: this.userName,
          amount: this.amount,
        });
        alert('Juego guardado');
      } catch (error) {
        this.$emit('error', error);
      }
      },
      exit(){
        window.location.reload();
      }
    },
  };
  </script>
  