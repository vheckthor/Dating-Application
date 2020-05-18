export interface IPhoto {
    id: number;
    url: string;
    description: string;
    photoUniqueIdentifier: string;
    dateAdded: Date;
    isMain: boolean;
}
