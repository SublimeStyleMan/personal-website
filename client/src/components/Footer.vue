<template>
  <footer
    class="bg-gradient-to-r from-blue-900 via-purple-900 to-blue-900 shadow-lg"
  >
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
      <div
        class="flex flex-col gap-4 py-6 md:flex-row md:items-center md:justify-between"
      >
        <span class="text-white font-bold text-lg">Portfolio | Profile</span>

        <div class="flex flex-wrap gap-x-6 gap-y-2">
          <a
            href="#skills"
            class="text-gray-200 hover:text-white text-sm font-medium transition"
          >
            Skills
          </a>
          <a
            href="#projects"
            class="text-gray-200 hover:text-white text-sm font-medium transition"
          >
            Projects
          </a>
          <a
            href="#resume"
            class="text-gray-200 hover:text-white text-sm font-medium transition"
          >
            Resume
          </a>
          <a
            href="#contact"
            class="text-gray-200 hover:text-white text-sm font-medium transition"
          >
            Contact
          </a>
        </div>
      </div>

      <p
        class="border-t border-white/20 py-4 text-center text-sm text-gray-300"
      >
        © 2026 Nathan Swindall. All rights reserved.
      </p>
    </div>
  </footer>
</template>

<style scoped>
a {
  transition: color 0.3s ease;
}
</style>
