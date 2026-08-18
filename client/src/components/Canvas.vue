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
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted } from "vue";
import Navbar from "./Navbar.vue";

const canvas = ref<HTMLCanvasElement | null>(null);
let animationFrameId: number;
const time = ref(0);

onMounted(() => {
  if (!canvas.value) return;

  const ctx = canvas.value.getContext("2d");
  if (!ctx) return;

  // Set canvas size
  canvas.value.width = window.innerWidth;
  canvas.value.height = window.innerHeight;

  const animate = () => {
    // Clear canvas with gradient background
    const gradient = ctx.createLinearGradient(0, 0, 0, canvas.value!.height);
    gradient.addColorStop(0, "#1a1a4d"); // Dark blue
    gradient.addColorStop(0.5, "#4a148c"); // Purple
    gradient.addColorStop(1, "#ff6b9d"); // Pink
    ctx.fillStyle = gradient;
    ctx.fillRect(0, 0, canvas.value!.width, canvas.value!.height);

    // Calculate sun position (arc path)
    const sunX = canvas.value!.width / 2 + Math.sin(time.value * 0.005) * 300;
    const sunY =
      canvas.value!.height / 2 + Math.cos(time.value * 0.005) * 200 + 100;

    // Draw sun
    const sunGradient = ctx.createRadialGradient(sunX, sunY, 0, sunX, sunY, 80);
    sunGradient.addColorStop(0, "#ffff00");
    sunGradient.addColorStop(0.7, "#ff8800");
    sunGradient.addColorStop(1, "rgba(255, 200, 0, 0)");

    // Path of the sun
    ctx.fillStyle = sunGradient;
    ctx.beginPath();
    ctx.arc(sunX, sunY, 80, 0, Math.PI * 2);
    ctx.fill();

    time.value++;
    animationFrameId = requestAnimationFrame(animate);
  };

  animate();

  // Handle resize
  const handleResize = () => {
    canvas.value!.width = window.innerWidth;
    canvas.value!.height = window.innerHeight;
  };
  window.addEventListener("resize", handleResize);

  onUnmounted(() => {
    cancelAnimationFrame(animationFrameId);
    window.removeEventListener("resize", handleResize);
  });
});
</script>

<style scoped>
.canvas-container {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  overflow: hidden;
  z-index: 40;
}

.navbar {
  z-index: 500;
}

canvas {
  display: block;
  position: absolute;
  top: 0;
  left: 0;
  z-index: 1;
  pointer-events: none;
}

.content {
  position: absolute;
  z-index: 60;
  width: 100%;
  height: 100%;
  top: 0;
  left: 0;
  overflow-y: auto;
  overflow-x: hidden;
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
