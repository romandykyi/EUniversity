/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      maxWidth: {
        '100px': '100px',
        '200px': '200px',
        '300px': '300px',
        '400px': '400px',
        '500px': '500px',
        '1100px': '1100px',
        '8px': '8px',
      },
      colors: {
        'text': 'var(--text)',
        'background': 'var(--background)',
        'theme': 'var(--theme)',
       },
    },
  },
  plugins: [],
}

