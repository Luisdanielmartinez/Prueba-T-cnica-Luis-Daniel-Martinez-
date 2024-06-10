<template>
  <div id="game-wheel">
    <div id="wheel_image">
      <div>&#9660;</div>
      <svg :style="spinStyle" id="wheel_svg" width="400" height="400" viewBox="0 0 100 100">
        <g>
          <circle cx="50" cy="50" r="48" stroke="black" stroke-width="1" fill="none" />
          <g v-for="(segment, index) in segments" :key="index">
            <path
              :d="describeArc(50, 50, 48, segment.startAngle, segment.endAngle)"
              :fill="targetSegment == null ? segment.color :targetSegment.color"
            />
            <text
              :x="50 + 40 * Math.cos((segment.startAngle + segment.endAngle) / 2 * Math.PI / 180)"
              :y="50 + 40 * Math.sin((segment.startAngle + segment.endAngle) / 2 * Math.PI / 180)"
              text-anchor="middle"
              alignment-baseline="middle"
              font-size="5"
              fill="white"
            >
              {{ targetSegment == null ? 0 :targetSegment.number }}
            </text>
          </g>
        </g>
      </svg>
    </div>
    <div id="wheel_meta">
      <div>
        <h3>Ultimos numeros</h3>
        <div v-for="(item, index) in lastNumbers" :key="index">
          <span :style="{ color: item.color }">{{ item.number }}</span>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: 'GameWheel',
  data() {
    return {
      currentSpin: 0,
      currentSpinAnimated: 0,
      rotateCycle: 3,
      rotateSpeed: 2,
      isSpinning: false,
      segments: [
        { number: 0, color: 'red', startAngle: 0, endAngle: 10 },
        { number: 1, color: 'black', startAngle: 10, endAngle: 20 },
        { number: 2, color: 'red', startAngle: 20, endAngle: 30 },
        { number: 3, color: 'black', startAngle: 30, endAngle: 40 },
        { number: 4, color: 'red', startAngle: 40, endAngle: 50 },
        { number: 5, color: 'black', startAngle: 50, endAngle: 60 },
        { number: 6, color: 'red', startAngle: 60, endAngle: 70 },
        { number: 7, color: 'black', startAngle: 70, endAngle: 80 },
        { number: 8, color: 'red', startAngle: 80, endAngle: 90 },
        { number: 9, color: 'black', startAngle: 90, endAngle: 100 },
        { number: 10, color: 'red', startAngle: 100, endAngle: 110 },
        { number: 11, color: 'black', startAngle: 110, endAngle: 120 },
        { number: 12, color: 'red', startAngle: 120, endAngle: 130 },
        { number: 13, color: 'black', startAngle: 130, endAngle: 140 },
        { number: 14, color: 'red', startAngle: 140, endAngle: 150 },
        { number: 15, color: 'black', startAngle: 150, endAngle: 160 },
        { number: 16, color: 'red', startAngle: 160, endAngle: 170 },
        { number: 17, color: 'black', startAngle: 170, endAngle: 180 },
        { number: 18, color: 'red', startAngle: 180, endAngle: 190 },
        { number: 19, color: 'red', startAngle: 190, endAngle: 200 },
        { number: 20, color: 'black', startAngle: 200, endAngle: 210 },
        { number: 21, color: 'red', startAngle: 210, endAngle: 220 },
        { number: 22, color: 'black', startAngle: 220, endAngle: 230 },
        { number: 23, color: 'red', startAngle: 230, endAngle: 240 },
        { number: 24, color: 'black', startAngle: 240, endAngle: 250 },
        { number: 25, color: 'red', startAngle: 250, endAngle: 260 },
        { number: 26, color: 'black', startAngle: 260, endAngle: 270 },
        { number: 27, color: 'red', startAngle: 270, endAngle: 280 },
        { number: 28, color: 'black', startAngle: 280, endAngle: 290 },
        { number: 29, color: 'black', startAngle: 290, endAngle: 300 },
        { number: 30, color: 'red', startAngle: 300, endAngle: 310 },
        { number: 31, color: 'black', startAngle: 310, endAngle: 320 },
        { number: 32, color: 'red', startAngle: 320, endAngle: 330 },
        { number: 33, color: 'black', startAngle: 330, endAngle: 340 },
        { number: 34, color: 'red', startAngle: 340, endAngle: 350 },
        { number: 35, color: 'black', startAngle: 350, endAngle: 360 },
      ],

      targetSegment: null,
      lastNumbers: [],
    };
  },
  computed: {
    spinStyle() {
      return `transform: rotate(${this.currentSpinAnimated}deg); transform-origin: 50% 50%`;
    },
  },
  methods: {
    describeArc(x, y, radius, startAngle, endAngle) {
      const start = this.polarToCartesian(x, y, radius, endAngle);
      const end = this.polarToCartesian(x, y, radius, startAngle);
      const largeArcFlag = endAngle - startAngle <= 180 ? '0' : '1';

      const d = [
        'M', start.x, start.y,
        'A', radius, radius, 0, largeArcFlag, 0, end.x, end.y,
        'L', x, y,
        'Z'
      ].join(' ');

      return d;
    },
    polarToCartesian(centerX, centerY, radius, angleInDegrees) {
      const angleInRadians = (angleInDegrees - 90) * Math.PI / 180.0;

      return {
        x: centerX + (radius * Math.cos(angleInRadians)),
        y: centerY + (radius * Math.sin(angleInRadians))
      };
    },
    spin() {
    this.isSpinning = true;
    this.startSpinAnimation();
   
},

    startSpinAnimation() {
      this.rotateCycle = 3;
      this.currentSpinAnimated = 0;
      this.animateSpin();
    },
    addLastNumber(number, color) {
      if (this.lastNumbers.length >= 5) {
        this.lastNumbers.shift(); // Eliminar el número más antiguo si hay más de 5
      }
      this.lastNumbers.push({ number, color });
    },
    animateSpin() {
      const targetRotation = 360 * this.rotateCycle + ((340 + 340) / 2) % 360;

    const duration = 3000;
    const start = performance.now();

    const animate = (time) => {
      const elapsed = time - start;
      if (elapsed < duration) {
        const progress = elapsed / duration;
        this.currentSpinAnimated = progress * targetRotation;
        requestAnimationFrame(animate);
      } else {
        this.currentSpinAnimated = targetRotation;
        axios.get('https://localhost:44351/api/roulette/GetRandomNumberAndColor')
        .then(response => {
          const { number, color } = response.data;
          this.targetSegment = this.segments.find(segment => segment.number === number);
          this.addLastNumber(number,color);
          this.targetSegment.color = color;
          this.endSpin();
          if (!this.targetSegment) {
            console.error('No se encontró el segmento de destino.');
            this.isSpinning = false; // Detener la animación si no se encontró el segmento
            return;
          }
          this.isSpinning = false;
        })
        .catch(error => {
          console.error("Error fetching spin result:", error);
          this.$emit('error', error);
          this.isSpinning = false;
        });
      
      }
    };
  
  requestAnimationFrame(animate);
  
},
  endSpin(){
    this.$emit('spin-end', this.targetSegment.number);
  }

  }
};
</script>

<style scoped>
#game-wheel {
  display: grid;
  grid-template-columns: 50% 50%;
}

@media (min-width: 768px) and (max-width: 1100px) {
  #game-wheel {
    grid-template-columns: 50% 50%;
  }
}

@media (max-width: 767px) {
  #game-wheel {
    grid-template-columns: 100%;
  }
}

#wheel_image {
  text-align: center;
  margin-top: 20px;
}

#wheel_meta {
  text-align: center;
  margin-top: 20px;
}

#wheel_svg {
  transition: transform 3s ease-out;
}
</style>
