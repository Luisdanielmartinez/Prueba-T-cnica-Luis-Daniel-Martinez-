// src/services/store.js

import { reactive } from 'vue';

const state = reactive({
  currentUser: null,
  currentNumber: null,
  lastNumbers: []
});

export const GameStoreChanger = {
  setCurrentUser(user) {
    state.currentUser = user;
  },
  setCurrentNumber(number) {
    state.currentNumber = number;
  },
  addLastNumber(number) {
    state.lastNumbers.push(number);
  },
  clearNumbers() {
    state.lastNumbers = [];
  }
};

export default state;
