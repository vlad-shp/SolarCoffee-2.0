<template>
	<v-dialog v-model="dialog" max-width="800px">
		<v-card v-if="shipment" class="py-4">
			<v-card-title> Receive shipment </v-card-title>
			<v-card-text>
				<v-form v-model="formValid">
					<v-row>
						<v-col cols="6">
							<v-text-field
								:value="productName"
								label="Product name"
								readonly
								prepend-inner-icon="mdi-alpha-t-box-outline"
							/>
						</v-col>
						<v-col cols="6">
							<v-text-field
								v-model="shipment.adjustment"
								label="Adjustment"
								:rules="[
									(v) =>
										(!isNaN(parseInt(v)) && v > 0) ||
										'Adjustment has to be more then 0',
								]"
								type="number"
								prepend-inner-icon="mdi-alpha-a-box-outline"
							/>
						</v-col>
					</v-row>
				</v-form>
			</v-card-text>
			<v-divider />

			<v-card-actions>
				<v-spacer />
				<v-btn color="blue darken-1" text @click="dialog = false">
					Close
				</v-btn>
				<v-btn
					color="blue darken-1"
					text
					:disabled="!formValid"
					@click="onSave"
				>
					Save
				</v-btn>
			</v-card-actions>
		</v-card>
	</v-dialog>
</template>

<script lang="ts">
import { IShipment } from "@/models/request/shipment";
import { Component, Vue, Prop, PropSync } from "vue-property-decorator";

@Component
export default class ReceiveShipmentDialog extends Vue {
	@Prop({ required: true }) shipment!: IShipment;
	@Prop({ required: true }) productName!: string;

	@PropSync("receiveShipmentDialogVisible", { required: true })
	dialog!: boolean;

	formValid = false;

	onSave(): void {
		if (this.shipment.adjustment > 0) {
			this.dialog = false;
			this.$emit("saveReceiveShipment", this.shipment);
		}
	}
}
</script>

<style scoped lang="scss">
@import "@/scss/global.scss";
</style>
