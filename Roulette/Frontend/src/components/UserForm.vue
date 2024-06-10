<template>
  <div class="container my-3">
    <h2>Iniciar Juego</h2>
    <div class="mb-3">
      <input v-model="userName" class="form-control" placeholder="Nombre de usuario" @input="getUser" />
    </div>
    <div v-if="initialAmount === 0 || initialAmount === null" class="mb-3">
  <input v-model.number="initialAmount" class="form-control" placeholder="Monto inicial" type="number" />
</div>
<div v-else class="mb-3">
  <input v-model.number="initialAmount" class="form-control" placeholder="Monto inicial" type="number" :disabled="!isSaveBalance" />
</div>
    <button @click="startGame" class="btn btn-primary me-2">Iniciar</button>
    <button @click="loadBalance" v-show="!isSaveBalance"  class="btn btn-primary">Cargar Saldo</button>
    <button @click="saveBalance" v-show="isSaveBalance" class="btn btn-success">Guardar Saldo</button>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  data() {
    return {
      userName: '',
      initialAmount: 0,
      isSaveBalance:false
    };
  },
  methods: {
    async startGame() {
      if(this.initialAmount == 0){
        this.$emit('error', 'No puedes inicial el juego con balance en 0.');
      }else{

        this.$emit('error', '');
      this.$emit('start-game', { userName: this.userName, initialAmount: this.initialAmount });
      }
    },
    async getUser() {
      if (this.userName.trim() === '') {
        return;
      }
      try {
        this.$emit('toggle-spinner', true);
        const response = await axios.get(`https://localhost:44351/api/roulette/GetUserByName/${this.userName}`);

        this.initialAmount = response.data.amount;
        this.$emit('toggle-spinner', false);
      } catch (error) {
        this.initialAmount = 0;
        this.$emit('toggle-spinner', false);
      }
    },
    loadBalance(){
      this.isSaveBalance=true;
    },
    async saveBalance() {
      try {
        if (this.userName.trim() === '') {
          this.$emit('error', "Tienes que agregar un nombre.");
        return;
      }
        const response = await axios.post('https://localhost:44351/api/roulette/save', {
          name: this.userName,
          amount: this.initialAmount,
        });
        this.$emit('load-balance', response.data);
      } catch (error) {
        this.$emit('error', error);
      }
    },
  },
};
</script>
