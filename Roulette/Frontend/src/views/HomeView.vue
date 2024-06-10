<template>
   <div class="container">
    <h1 class="my-3">Juego de Ruleta</h1>
  
    <UserForm v-if="isVisibleUserForm" @start-game="startGame" @load-balance="loadBalance" @toggle-spinner="toggleSpinner" @error="showError"/>
    <GameAlert v-if="errorMessage" :message="errorMessage" @close="errorMessage = ''"/>
    <div v-if="user">
      <p>Bienvenido, {{ user.userName }}. Saldo: {{ user.amount }}</p>
      <BetForm @place-bet="placeBet" :isDisabled="isBetting" @toggle-spinner="toggleSpinner" @error="showError"/>
      <GameWheel ref="gameWheel" @spin-end="onSpinEnd" @error="showError"/>
    </div>
    <BetResult v-if="betResult" :result="betResult" :prize="prize" :userName="user.userName" :amount="user.amount" @error="showError"/>
    <!-- <div class="roulette-wheel my-3">
      <div class="circle">
        <div class="number">{{ rouletteNumber }}</div>
        <div class="color" :style="{ backgroundColor: rouletteColor }"></div>
      </div>
    </div> -->
    <div v-if="isSpinning" class="spinner-overlay">
      <div class="spinner-border text-primary" role="status">
        <span class="visually-hidden">Loading...</span>
      </div>
    </div>
  </div>
  </template>
  
  <script>
  import UserForm from '../components/UserForm.vue';
  import BetForm from '../components/BetForm.vue';
  import BetResult from '../components/BetResult.vue';
  import GameWheel from '../components/GameWheel.vue';
  import GameAlert from '../components/GameAlert.vue';
  import axios from 'axios';
  
  export default {
    components: {
      UserForm,
      BetForm,
      BetResult,
      GameWheel,
      GameAlert,
    },
    data() {
      return {
        user: null,
        betResult: null,
        prize: 0,
        isSpinning: false,
        isBetting: false,
        isVisibleUserForm:true,
        errorMessage: '',
      };
    },
    methods: {
      async startGame(user) {
        this.user = user;
        this.user.amount = user.initialAmount;
        this.isVisibleUserForm=false;
      },
      async loadBalance(user) {
        this.user = user;
      },
      async placeBet(bet) {
        this.isBetting = true; // Desactivar el botón
      this.$refs.gameWheel.spin();
      try {
        if(this.user.amount == 0){
          this.showError("No tiene monto suficiente para seguir apontando");
          this.isVisibleUserForm=true;
          return;
        }else{
          const response = await axios.post('https://localhost:44351/api/roulette/bet', {
          userName: this.user.userName,
          amount: bet.amount,
          color: bet.color,
          number: bet.number,
          isEven: bet.isEven,
        });
        this.prize = response.data.prize;
        this.user.amount = this.prize == 0 ? this.user.amount - bet.amount : this.user.amount+this.prize;
        }
   
      } catch (error) {
        console.error("Error placing bet:", error);
        this.errorMessage = error; 
      } finally {
        this.isBetting = false; // Activar el botón
      }
      },
      onSpinEnd(numberWinner){
        this.betResult=numberWinner;
      },
      toggleSpinner(show) {
      this.isSpinning = show;
    },
    showError(message) {
      this.errorMessage = message;
    }
    },
  };
  </script>
  <style scoped>
  .spinner-overlay {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    display: flex;
    align-items: center;
    justify-content: center;
    background: rgba(255, 255, 255, 0.8);
    z-index: 1000;
  }
  </style>