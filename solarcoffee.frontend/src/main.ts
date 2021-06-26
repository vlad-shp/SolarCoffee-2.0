import Vue from "vue";
import App from "./App.vue";
import vuetify from "./plugins/vuetify";
import router from "./router";
import CustomerService from "@/services/customer-service";
import ProductService from "@/services/product-service";
import InventoryService from "./services/inventory-service";
import OrderService from "./services/order-service";
import OrderSettingsService from "./services/order-settings-service";
import moment from "moment";

Vue.config.productionTip = false;

Vue.filter("price", function (price: number) {
	return "$" + price.toFixed(2);
});

Vue.filter("humanizeDate", function (date: Date) {
	return moment(date).format("MMMM Do YYYY");
});

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
