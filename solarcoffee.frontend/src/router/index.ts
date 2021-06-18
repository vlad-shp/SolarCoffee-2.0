import Vue from "vue";
import VueRouter, { RouteConfig } from "vue-router";

Vue.use(VueRouter);

const routes: Array<RouteConfig> = [
	{
		path: "/",
		name: "home",
		component: () => import("@/views/Home.vue"),
	},
	{
		path: "/inventory",
		name: "inventory",
		//component: () => import("@/views/Inventory.vue"),
	},
	{
		path: "/customers",
		name: "customers",
		component: () => import("@/views/Customers.vue"),
	},
	{
		path: "/orders",
		name: "orders",
		//component: () => import("@/views/Orders.vue"),
	},
	{
		path: "/products",
		name: "products",
		component: () => import("@/views/Products.vue"),
	},
	{
		path: "/order/new",
		name: "new-order",
		//component: () => import("@/views/CreateInvoice.vue"),
	},
	{
		path: "/order/settings",
		name: "order-settings",
		//component: () => import("@/views/CreateInvoice.vue"),
	},
];

const router = new VueRouter({
	mode: "history",
	base: process.env.BASE_URL,
	routes,
});

export default router;
