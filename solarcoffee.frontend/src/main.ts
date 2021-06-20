import Vue from "vue";
import App from "./App.vue";
import vuetify from "./plugins/vuetify";
import router from "./router";
import CustomerService from "@/services/customer-service";
import ProductService from "@/services/product-service";
import InventoryService from "./services/inventory-service";
import OrderService from "./services/order-service";
import OrderSettingsService from "./services/order-settings-service";

Vue.config.productionTip = false;

new Vue({
	vuetify,
	router,
	render: (h) => h(App),
	provide: {
		customerService: new CustomerService(),
		productService: new ProductService(),
		inventoryService: new InventoryService(),
		orderService: new OrderService(),
		orderSettingsService: new OrderSettingsService(),
	},
}).$mount("#app");
