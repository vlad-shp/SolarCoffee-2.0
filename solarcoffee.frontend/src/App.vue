<template>
	<v-app>
		<div class="app-menu">
			<ToolBar />
		</div>
		<div class="app-content">
			<router-view />
		</div>
	</v-app>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import ToolBar from "./components/ToolBar.vue";

@Component({ components: { ToolBar } })
export default class App extends Vue {}
</script>
