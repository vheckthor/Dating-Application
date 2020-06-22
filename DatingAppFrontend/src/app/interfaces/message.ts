export interface IMessage {
    uniqueId: string;
    senderUniqueId: string;
    senderKnownAs: string;
    senderPhotoUrl: string;
    recipientUniqueId: string;
    recipientKnownAs: string;
    recipientPhotoUrl: string;
    content: string;
    isRead: boolean;
    dateRead: Date;
    messageSent: Date;
}


