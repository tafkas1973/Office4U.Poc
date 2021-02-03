export class ToastrServiceMock {

    constructor() {
        this.error = jest.fn();
    }

    error(message: string) {
        throw new Error('Method not implemented.');
    }
}