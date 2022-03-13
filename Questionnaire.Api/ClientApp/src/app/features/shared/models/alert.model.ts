export class Alert {
    id: string;
    type: AlertType;
    message: string;
    keepAlertAfterRouteChange: boolean;

    constructor(init?: Partial<Alert>) {
        Object.assign(this, init);
    }
}

export enum AlertType {
    Success,
    Error
}
