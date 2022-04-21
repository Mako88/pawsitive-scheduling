import { svelte } from '@sveltejs/vite-plugin-svelte';
import { defineConfig } from 'vite';
import tsconfigPaths from "vite-tsconfig-paths";

export default defineConfig({
    server: {
        port: 5000,
    },
    
    plugins: [tsconfigPaths(), svelte()],
});
