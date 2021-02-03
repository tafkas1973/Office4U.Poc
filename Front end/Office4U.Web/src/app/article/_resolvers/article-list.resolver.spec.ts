import { ActivatedRouteSnapshot } from "@angular/router";
import { of } from "rxjs";
import { ArticleServiceMock } from "../../mocks/article-service.mock";
import { ToastrServiceMock } from "../../mocks/toatr-service.mock";
import { RouterMock } from "../../mocks/router.mock";
import { Article } from "../../shared/models/article";
import { PaginatedResult } from "../../shared/models/pagination";
import { ArticleParams } from "../../shared/models/articleParams";
import { ArticleListResolver } from "./article-list.resolver";

describe('ArticleListResolver', () => {
    let articleServiceMock: ArticleServiceMock;
    let routerMock: RouterMock;
    let toastrServiceMock: ToastrServiceMock;
    let resolver: ArticleListResolver;
    let route: ActivatedRouteSnapshot;

    beforeEach(() => {
        const articleParamsMock: Partial<ArticleParams> = {
            pageNumber: 1,
            pageSize: 5
        };
        const articlesMock: PaginatedResult<Array<Article>> = {
            result: [
                {
                    id: 1, code: 'art code 1', name1: 'art 1 name', supplierId: 'sup id 1', supplierReference: 'sup ref 1', purchasePrice: 10.00, unit: 'ST', photoUrl: 'art 1 photo 1',
                    photos: [{ id: 1, url: 'art 1 photo 1', isMain: true }]
                },
                {
                    id: 2, code: 'art code 2', name1: 'art 2 name', supplierId: 'sup id 2', supplierReference: 'sup ref 2', purchasePrice: 15.00, unit: 'ST', photoUrl: 'art 2 photo 1',
                    photos: [{ id: 2, url: 'art 2 photo 1', isMain: true }]
                },
                {
                    id: 3, code: 'art code 3', name1: 'art 3 name', supplierId: 'sup id 3', supplierReference: 'sup ref 3', purchasePrice: 20.00, unit: 'ST', photoUrl: 'art 3 photo 1',
                    photos: [{ id: 3, url: 'art 3 photo 1', isMain: true }]
                }
            ],
            pagination: {
                currentPage: 1,
                itemsPerPage: 5,
                totalItems: 3,
                totalPages: 1
            }
        };

        articleServiceMock = new ArticleServiceMock();
        articleServiceMock.getArticleParams = jest.fn().mockReturnValue(articleParamsMock);
        articleServiceMock.getArticles = jest.fn().mockReturnValue(of(articlesMock));
        routerMock = new RouterMock();
        toastrServiceMock = new ToastrServiceMock();
        route = new ActivatedRouteSnapshot();
        resolver = new ArticleListResolver(articleServiceMock as any, routerMock as any, toastrServiceMock as any);
    });

    test('should return articles when route is resolved', async () => {
        const paginatedResult = await resolver.resolve(route).toPromise();

        expect(paginatedResult.result.length).toEqual(3);
        expect(paginatedResult.result[0].name1).toEqual('art 1 name');
    });
});
