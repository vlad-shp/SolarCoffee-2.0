import Vue from "vue";
import App from "./App.vue";
import vuetify from "./plugins/vuetify";
import router from "./router";
import CustomerService from "@/services/customer-service";

Vue.config.productionTip = false;

new Vue({
	vuetify,
	router,
	render: (h) => h(App),
	provide: {
		customerService: new CustomerService(),
	},
}).$mount("#app");
