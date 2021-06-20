import IDelivery from "./delivery";
import IDiscount from "./discount";
import IPayment from "./payment";

export default interface IOrderSettings {
	payments: IPayment[];
	discounts: IDiscount[];
	deliveries: IDelivery[];
}
