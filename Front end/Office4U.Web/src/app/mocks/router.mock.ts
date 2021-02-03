export class RouterMock {
    constructor() {
        this.getCurrentNavigation = jest.fn();
    }

    getCurrentNavigation() {
        throw new Error('Method not implemented.');
    }
}
