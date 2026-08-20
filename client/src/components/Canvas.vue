<template>
  <div class="canvas-container">
    <canvas ref="canvas"></canvas>
    <div class="content">
      <Navbar class="navbar" />
      <h1 class="name-title">Nathan Swindall</h1>
      <div class="info-boxes">
        <div class="info-box left-box">
          <p>
            "A passionate developer with a focus in Web Development who dabbles
            in video game modding and 3D Art."
          </p>
        </div>
        <div class="info-box right-box">
          <h3>Interests</h3>
          <ul>
            <li>Cats</li>
            <li>Web Development</li>
            <li>Video Games</li>
            <li>Game Modding</li>
            <li>3D Art</li>
          </ul>
        </div>
      </div>
      <div class="additional-content">
        <!-- Add more content sections here -->
        <section class="content-section">
          <h2>Projects</h2>
          <p>Your projects will go here</p>
        </section>
        <section class="content-section">
          <h2>Skills</h2>
          <p>Your skills will go here</p>
        </section>
        <section class="content-section">
          <h2>Contact</h2>
          <p>Your contact info will go here</p>
        </section>
      </div>
      <Footer />
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted } from "vue";
import Footer from "./Footer.vue";
import Navbar from "./Navbar.vue";
import {
  animeService,
  type AnimeCharacter,
} from "../Services/AnimeCharacterService.ts";

// Data
const canvas = ref<HTMLCanvasElement | null>(null);
let animationFrameId: number;
const time = ref(0);
const slowedTimeValue = ref(0);
const myCharacter = ref<AnimeCharacter | null>(null);
let cloudOffset = 0;

function drawClouds(
  ctx: CanvasRenderingContext2D,
  canvasWidth: number,
  cloudY: number,
  offset: number,
): void {
  const clouds = [
    { x: canvasWidth * 0.05, width: 170, height: 52, speed: 0.22 },
    { x: canvasWidth * 0.2, width: 210, height: 60, speed: 0.16 },
    { x: canvasWidth * 0.38, width: 150, height: 48, speed: 0.28 },
    { x: canvasWidth * 0.55, width: 230, height: 65, speed: 0.18 },
    { x: canvasWidth * 0.7, width: 175, height: 55, speed: 0.24 },
    { x: canvasWidth * 0.85, width: 160, height: 50, speed: 0.3 },
    { x: canvasWidth * 0.98, width: 220, height: 58, speed: 0.2 },
  ];

  ctx.save();
  ctx.fillStyle = "rgba(255, 255, 255, 0.85)";
  ctx.shadowColor = "rgba(0, 0, 0, 0.2)";
  ctx.shadowBlur = 12;

  clouds.forEach(({ x, width, height, speed }) => {
    const cloudX = ((x + offset * speed) % (canvasWidth + width)) - width / 2;

    ctx.beginPath();
    ctx.ellipse(cloudX, cloudY, width / 2, height / 2, 0, 0, Math.PI * 2);
    ctx.ellipse(
      cloudX - width * 0.2,
      cloudY - height * 0.35,
      width * 0.25,
      height * 0.55,
      0,
      0,
      Math.PI * 2,
    );
    ctx.ellipse(
      cloudX + width * 0.15,
      cloudY - height * 0.45,
      width * 0.3,
      height * 0.65,
      0,
      0,
      Math.PI * 2,
    );
    ctx.fill();
  });

  ctx.restore();
}

// handle resize
const handleResize = () => {
  if (!canvas.value) return;

  canvas.value.width = window.innerWidth;
  canvas.value.height = window.innerHeight;
};

// onMounted
onMounted(async () => {
  if (!canvas.value) return;

  const ctx = canvas.value.getContext("2d");
  if (!ctx) return;

  // Set canvas size
  canvas.value.width = window.innerWidth;
  canvas.value.height = window.innerHeight;
  let sunStopped = false;
  let stoppedSunX = 0;

  const animate = () => {
    slowedTimeValue.value = time.value / (60 + 0.4 * time.value);

    // Get the sunx and suny
    const calculatedSunX =
      canvas.value!.width / 2 + Math.sin(slowedTimeValue.value) * 300;
    const calculatedSunY =
      canvas.value!.height / 2 + Math.cos(slowedTimeValue.value) * 200 + 100;

    // Freeze the sun when it first reaches the cloud layer.
    if (!sunStopped && calculatedSunY <= 310) {
      sunStopped = true;
      stoppedSunX = calculatedSunX;
    }

    const sunX = sunStopped ? stoppedSunX : calculatedSunX;
    const sunY = sunStopped ? 320 : calculatedSunY;

    // Clear canvas with gradient background
    const gradient = ctx.createLinearGradient(0, 0, 0, canvas.value!.height);
    gradient.addColorStop(0, "#1a1a4d"); // Dark blue
    gradient.addColorStop(0.5, "#4a148c"); // Purple
    gradient.addColorStop(1, "#ff6b9d"); // Pink
    ctx.fillStyle = gradient;
    ctx.fillRect(0, 0, canvas.value!.width, canvas.value!.height);

    // Draw sun
    const sunRadius = 120;
    const sunGradient = ctx.createRadialGradient(
      sunX,
      sunY,
      0,
      sunX,
      sunY,
      sunRadius,
    );
    sunGradient.addColorStop(0, "#ffff00");
    sunGradient.addColorStop(0.7, "#ff8800");
    sunGradient.addColorStop(1, "rgba(255, 200, 0, 0)");

    // Draw the sun before the clouds so it can disappear behind them.
    ctx.fillStyle = sunGradient;
    ctx.beginPath();
    ctx.arc(sunX, sunY, sunRadius, 0, Math.PI * 2);
    ctx.fill();

    // Move the clouds across the y = 320 layer every frame.
    drawClouds(ctx, canvas.value!.width, 320, cloudOffset);
    cloudOffset += 1;

    time.value++;
    animationFrameId = requestAnimationFrame(animate);
  };

  animate();

  // Handle resize listener
  window.addEventListener("resize", handleResize);

  // Get anime character
  myCharacter.value = await animeCharacter("Sailor");
});

// Handle unMounted
onUnmounted(() => {
  cancelAnimationFrame(animationFrameId);
  window.removeEventListener("resize", handleResize);
});

// Get anime Character
async function animeCharacter(character: string) {
  const characters = (await animeService.searchCharacters(
    character,
    1,
    20,
  )) as AnimeCharacter[];

  return characters[0];
}
</script>

<style scoped>
.canvas-container {
  position: relative;
  top: 0;
  left: 0;
  width: 100%;
  min-height: 100vh;
  background: linear-gradient(
    to bottom,
    #1a1a4d 0,
    #4a148c 50vh,
    #ff6b9d 100vh
  );
}

.navbar {
  z-index: 500;
}

canvas {
  display: block;
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100vh;
  z-index: 1;
  pointer-events: none;
}

.content {
  position: relative;
  z-index: 60;
  width: 100%;
  top: 0;
  left: 0;
  overflow: visible;
}

/* Dark scrollbar styling with visible arrow buttons */
.content::-webkit-scrollbar {
  width: 12px;
}

.content::-webkit-scrollbar-track {
  background: rgba(0, 0, 0, 0.1);
}

.content::-webkit-scrollbar-thumb {
  background: rgba(60, 60, 80, 0.8);
  border-radius: 1px;
}

.content::-webkit-scrollbar-thumb:hover {
  background: rgba(80, 80, 100, 0.9);
}

/* Arrow buttons */
.content::-webkit-scrollbar-button {
  background: rgba(60, 60, 80, 0.8);
  height: 14px;
  background-size: contain;
  background-repeat: no-repeat;
  background-position: center;
}

.content::-webkit-scrollbar-button:hover {
  background-color: rgba(80, 80, 100, 0.9);
}

.content::-webkit-scrollbar-button:vertical:start:decrement {
  display: block;
  background-image: url('data:image/svg+xml;utf8,<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 12 12"><polygon points="6,2 10,8 2,8" fill="rgba(200,200,220,0.7)"/></svg>');
}

.content::-webkit-scrollbar-button:vertical:end:increment {
  display: block;
  background-image: url('data:image/svg+xml;utf8,<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 12 12"><polygon points="6,10 10,4 2,4" fill="rgba(200,200,220,0.7)"/></svg>');
}

.name-title {
  position: absolute;
  top: 0;
  left: 50%;
  transform: translateX(-50%);
  font-size: 64px;
  font-weight: 300;
  color: #ffffff;
  margin: 0;
  letter-spacing: 2px;
  text-shadow: 0 2px 10px rgba(0, 0, 0, 0.3);
  z-index: 20;
  padding: 80px 0 60px 0;
  text-align: center;
}

.info-boxes {
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
  align-items: flex-start;
  justify-content: center;
  padding: 60px 60px;
  box-sizing: border-box;
  gap: 40px;
  margin-top: 300px;
}

@media (max-width: 768px) {
  .info-boxes {
    flex-direction: column;
    align-items: center;
    padding: 40px 20px;
  }
}

.info-box {
  background: rgba(30, 20, 80, 0.6);
  backdrop-filter: blur(10px);
  border: 1px solid rgba(255, 255, 255, 0.2);
  border-radius: 20px;
  padding: 40px;
  color: #ffffff;
  font-family: "Arial", sans-serif;
  max-width: 350px;
  box-shadow: 0 8px 32px rgba(0, 0, 0, 0.3);
  flex: 1;
}

.left-box {
  text-align: center;
}

.left-box p {
  margin: 0;
  font-size: 18px;
  line-height: 1.6;
}

.right-box h3 {
  margin: 0 0 20px 0;
  font-size: 24px;
  border-bottom: 2px solid #6366f1;
  padding-bottom: 10px;
}

.right-box ul {
  list-style: none;
  padding: 0;
  margin: 0;
}

.right-box li {
  font-size: 16px;
  padding: 8px 0;
}

.additional-content {
  padding: 60px;
  box-sizing: border-box;
}

.content-section {
  background: rgba(30, 20, 80, 0.6);
  backdrop-filter: blur(10px);
  border: 1px solid rgba(255, 255, 255, 0.2);
  border-radius: 20px;
  padding: 40px;
  margin: 40px 0;
  color: #ffffff;
  font-family: "Arial", sans-serif;
  box-shadow: 0 8px 32px rgba(0, 0, 0, 0.3);
}

.content-section h2 {
  margin-top: 0;
  font-size: 32px;
  color: #ffffff;
}

.content-section p {
  font-size: 16px;
  line-height: 1.8;
  margin: 0;
}
</style>
