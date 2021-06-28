<template>
	<v-container style="height: 400px">
		<v-row class="fill-height" align-content="center" justify="center">
			<v-col class="text-subtitle-1 text-center" cols="12">
				{{ msg }}
			</v-col>
			<v-col cols="6">
				<v-progress-linear indeterminate></v-progress-linear>
			</v-col>
		</v-row>
	</v-container>
</template>

<script lang="ts">
import { Component, Vue, Prop } from "vue-property-decorator";

@Component
export default class Loading extends Vue {
	@Prop({ required: true })
	msg!: string;
}
</script>

<style scoped lang="scss">
@import "@/scss/global.scss";
</style>
