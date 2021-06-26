export default interface IProduct {
	id: number;
	name: string;
	description: string;
	price: number;
	isTaxable: boolean;
	isArchived: boolean;
	createdOn: Date;
	updatedOn: Date;
}
