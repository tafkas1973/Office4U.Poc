import { of } from "rxjs";
import { ArticleServiceMock } from "../../mocks/article-service.mock";
import { RouterMock } from "../../mocks/router.mock";
import { ToastrServiceMock } from "../../mocks/toatr-service.mock";
import { Article } from "../../shared/models/article";
import { ArticleDetailResolver } from "./article-detail.resolver";

describe('ArticleDetailResolver', () => {
    let articleServiceMock: ArticleServiceMock;
    let routerMock: RouterMock;
    let toastrServiceMock: ToastrServiceMock;
    let resolver: ArticleDetailResolver;
    let route: any;

    beforeEach(() => {
        const articleMock: Article = {
            id: 1, code: 'art code 1', name1: 'art 1 name', supplierId: 'sup id 1', supplierReference: 'sup ref 1', purchasePrice: 10.00, unit: 'ST', photoUrl: 'art 1 photo 1',
            photos: [{ id: 1, url: 'art 1 photo 1', isMain: true }]
        };

        articleServiceMock = new ArticleServiceMock();
        articleServiceMock.getArticle = jest.fn().mockReturnValue(of(articleMock));
        routerMock = new RouterMock();
        route = { params: { id: articleMock.id } };
        toastrServiceMock = new ToastrServiceMock();
        resolver = new ArticleDetailResolver(articleServiceMock as any, routerMock as any, toastrServiceMock as any);
    });

    test('should return article when route is resolved', async () => {
        const result = await resolver.resolve(route as any).toPromise();

        expect(result.id).toEqual(1);
        expect(result.code).toEqual('art code 1');
        expect(result.name1).toEqual('art 1 name');
        expect(result.supplierId).toEqual('sup id 1');
        expect(result.supplierReference).toEqual('sup ref 1');
        expect(result.purchasePrice).toEqual(10.00);
        expect(result.unit).toEqual('ST');
        expect(result.photoUrl).toEqual('art 1 photo 1');
        expect(result.photos.length).toEqual(1);
        expect(result.photos[0].id).toEqual(1);
        expect(result.photos[0].url).toEqual('art 1 photo 1');
        expect(result.photos[0].isMain).toEqual(true);
    });
});