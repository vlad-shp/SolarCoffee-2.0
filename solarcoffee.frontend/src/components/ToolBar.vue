<template>
	<div>
		<v-toolbar
			dark
			prominent
			src="https://cdn.vuetifyjs.com/images/backgrounds/vbanner.jpg"
		>
			<v-toolbar-title>Solar Coffee 2.0</v-toolbar-title>

			<v-spacer></v-spacer>

			<v-btn icon>
				<v-icon>mdi-export</v-icon>
			</v-btn>

			<template v-slot:extension>
				<v-tabs align-with-title>
					<v-tab :to="{ name: 'home' }">Home</v-tab>
					<v-tab :to="{ name: 'customers' }">Manage Customers</v-tab>
					<v-tab :to="{ name: 'products' }">Products</v-tab>
					<v-tab :to="{ name: 'inventory' }">Inventory</v-tab>
					<v-tab :to="{ name: 'orders' }">Orders</v-tab>
				</v-tabs>
			</template>
		</v-toolbar>
	</div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";

@Component
export default class ToolBar extends Vue {
	drawer = false;
}
</script>
