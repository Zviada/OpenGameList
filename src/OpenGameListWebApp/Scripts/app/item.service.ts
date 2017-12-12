import { Injectable } from "@angular/core";
import { Http, Response } from "@angular/http";
import { Observable } from "rxjs/Observable";
import { Item } from "./item";

@Injectable()
export class ItemService {

    constructor(private http: Http) { }

    private baseUrl = 'api/items/'; //webapi URL

	//calls the [GET] /api/items/GetLatest/{n}
	getLatest(num?: number) {
        return this.getItems('GetLatest', num);
    }

	//calls the [GET] /api/items/GetMostViewed/{n}
	getMostViewed(num?: number) {
	    return this.getItems('GetMostViewed', num);
    }

	//calls the [GET] /api/items/GetRandom/{n}
    getRandom(num?: number) {
        return this.getItems('GetRandom', num);
    }

	//calls the [GET] /api/items/{id}
	get(id: number) {
	    if (id == null) {
	        throw new Error('id is required');
        }

        const url = this.baseUrl + id;
	    return this.http.get(url)
	        .map(res => <Item>res.json())
	        .catch(this.handleError);
	}

	//makes a GET call to /api/items/{actionName}/{n}
    private getItems(actionName: string, num?: number) {
        const url = this.baseUrl + actionName + '/' + (num != null ? num : '');
        return this.http.get(url)
            .map(response => <Item>response.json())
            .catch(this.handleError);
    }

    private handleError(error: Response) {
        //output errors to the console
        console.error(error);
        return Observable.throw(error.json().error || "Server error");
    }
}