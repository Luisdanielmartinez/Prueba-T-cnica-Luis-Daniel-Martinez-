<template>
  <div class="container my-3">
    <h2>Realizar Apuesta</h2>
    <div class="mb-3">
      <input v-model.number="amount" class="form-control" placeholder="Monto de apuesta" type="number" />
    </div>
    <div class="mb-3">
      <input v-model.number="number" class="form-control" placeholder="Número (0-36)" type="number" />
    </div>
    <div class="mb-3 form-check">
      <input v-model="isEven" class="form-check-input" type="checkbox" id="isEven" />
      <label class="form-check-label" for="isEven">Pares/Impares</label>
    </div>
    <div class="mb-3">
      <label for="color" class="form-label">Seleccionar Color</label>
      <select v-model="color" class="form-control" id="color">
        <option value="">Ninguno</option>
        <option value="Red">Rojo</option>
        <option value="Black">Negro</option>
      </select>
    </div>
    <button @click="placeBet" class="btn btn-success" :disabled="isDisabled">Apostar</button>
  </div>
</template>

<script>
export default {
  name: 'GameAlert',
  props: {
    isDisabled: {
      type: Boolean,
      default: false
    }
  },
  data() {
    return {
      amount: 0,
      color: '',  // Default color
      number: null,
      isEven: null,
    };
  },
  methods: {
    async placeBet() {
      if (this.amount === null || this.amount === 0 || this.number === null || this.number === 0 || isNaN(this.amount) || isNaN(this.number)) {
        this.$emit('error', 'El Monto de apuesta no puede estar el cero, tampoco el numero de apostar.');
        return;
      }
      
      try {
        this.$emit('error', '');
        this.$emit('place-bet', {
          amount: this.amount,
          color: this.color,
          number: this.number,
          isEven: this.isEven,
        });
      } catch (error) {
        this.$emit('error', error);
      }
    },
  },
};
</script>
