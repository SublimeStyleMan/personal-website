<script setup lang="ts">
import { computed, ref } from "vue";

const isOpen = ref(false);

// weather variable to dynamically change the background color of the navbar based on the weather condition
const weather = ref("clear");

// Get location and fetch weather data
navigator.geolocation.getCurrentPosition(async (position) => {
  console.log("Position:", position);

  const { latitude, longitude } = position.coords;
  const apiKey = "YOUR_OPENWEATHERMAP_API_KEY"; // Replace with your OpenWeatherMap API key
  const apiUrl = `https://api.openweathermap.org/data/2.5/weather?lat=${latitude}&lon=${longitude}&appid=${apiKey}`;

  try {
    const response = await fetch(apiUrl);
    const data = await response.json();
    weather.value = data.weather[0].main.toLowerCase();
  } catch (error) {
    console.error("Error fetching weather data:", error);
  }
});

// Changes background color based on the weather condition
const weatherBackground = computed(() => {
  switch (weather.value) {
    case "rain":
      return "bg-slate-900";

    case "snow":
      return "bg-blue-100";

    case "clear":
      return "bg-sky-400";

    case "cloudy":
      return "bg-slate-500";

    default:
      return "bg-gray-900";
  }
});
</script>

<template>
  <nav
    class="sticky top-0 z-50 bg-gradient-to-r from-blue-900 via-purple-900 to-blue-900 shadow-lg"
  >
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
      <div class="flex justify-between items-center h-16">
        <!-- Logo/Branding -->
        <div class="flex-shrink-0">
          <span class="text-white font-bold text-xl">Portfolio | Profile</span>
        </div>

        <!-- Desktop Navigation -->
        <div class="hidden md:flex space-x-8">
          <a
            href="#skills"
            class="text-gray-200 hover:text-white px-3 py-2 text-sm font-medium transition"
          >
            Skills
          </a>
          <a
            href="#projects"
            class="text-gray-200 hover:text-white px-3 py-2 text-sm font-medium transition"
          >
            Projects
          </a>
          <a
            href="#resume"
            class="text-gray-200 hover:text-white px-3 py-2 text-sm font-medium transition"
          >
            Resume
          </a>
          <a
            href="#contact"
            class="text-gray-200 hover:text-white px-3 py-2 text-sm font-medium transition"
          >
            Contact
          </a>
        </div>

        <!-- Mobile Menu Button -->
        <div class="md:hidden">
          <button
            @click="isOpen = !isOpen"
            class="text-gray-200 hover:text-white focus:outline-none"
          >
            <svg
              class="w-6 h-6"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M4 6h16M4 12h16M4 18h16"
              />
            </svg>
          </button>
        </div>
      </div>

      <!-- Mobile Menu -->
      <div v-if="isOpen" class="md:hidden pb-4">
        <a
          href="#skills"
          class="text-gray-200 hover:text-white block px-3 py-2 text-sm font-medium"
        >
          Skills
        </a>
        <a
          href="#projects"
          class="text-gray-200 hover:text-white block px-3 py-2 text-sm font-medium"
        >
          Projects
        </a>
        <a
          href="#resume"
          class="text-gray-200 hover:text-white block px-3 py-2 text-sm font-medium"
        >
          Resume
        </a>
        <a
          href="#contact"
          class="text-gray-200 hover:text-white block px-3 py-2 text-sm font-medium"
        >
          Contact
        </a>
      </div>
    </div>
  </nav>
</template>

<style scoped>
a {
  transition: color 0.3s ease;
}
</style>
