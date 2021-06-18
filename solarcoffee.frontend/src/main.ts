import Vue from "vue";
import App from "./App.vue";
import vuetify from "./plugins/vuetify";
import router from "./router";
import CustomerService from "@/services/customer-service";
import ProductService from "@/services/product-service";

Vue.config.productionTip = false;

new Vue({
	vuetify,
	router,
	render: (h) => h(App),
	provide: {
		customerService: new CustomerService(),
		productService: new ProductService(),
	},
}).$mount("#app");
