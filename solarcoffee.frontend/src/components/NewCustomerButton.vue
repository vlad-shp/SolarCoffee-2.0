<template>
	<v-dialog v-model="dialog" max-width="400px">
		<template v-slot:activator="{ on }">
			<v-btn elevation="0" color="secondary" v-on="on">
				Add New Customer
				<v-icon right>mdi-plus</v-icon>
			</v-btn>
		</template>
	</v-dialog>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";

@Component
export default class NewCustomerButton extends Vue {
	dialog = false;
}
</script>

<style scoped></style>
