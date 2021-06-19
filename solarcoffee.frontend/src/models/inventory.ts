import IProduct from "./product";

export default interface IInventory {
	id: number;
	quantityOnHand: number;
	idealQuantity: number;
	product: IProduct;
	createdOn: Date;
	updatedOn: Date;
}
