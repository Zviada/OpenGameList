import { Injectable } from "@angular/core";
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { Observable } from "rxjs/Observable";
import { Item } from "./item";

@Injectable()
export class ItemService {

    private baseUrl = 'api/items/'; //webapi URL

    constructor(private http: Http) { }

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
	        .map(res => res.json())
	        .catch(this.handleError);
    }

    //calls the [POST] /api/items/ Web API method to add a new item
    add(item: Item) {
        return this.http.post(this.baseUrl, JSON.stringify(item), this.getRequestOptions())
            .map(response => response.json())
            .catch(this.handleError);
    }

    //calls the [PUT] /api/items/{id} Web API method to update an existing item
    update(item: Item) {
        const url = this.baseUrl + item.Id;
        return this.http.put(url, JSON.stringify(item), this.getRequestOptions())
            .map(response => response.json())
            .catch(this.handleError);
    }

    //calls the [DELETE] /api/items/{id} Web API method to delete the item with given id
    delete(id: number) {
        const url = this.baseUrl + id;
        return this.http.delete(url).catch(this.handleError);
    }

	//makes a GET call to /api/items/{actionName}/{n}
    private getItems(actionName: string, num?: number) {
        const url = this.baseUrl + actionName + '/' + (num != null ? num : '');
        return this.http.get(url)
            .map(response => response.json())
            .catch(this.handleError);
    }

    private getRequestOptions() {
        return new RequestOptions({
            headers: new Headers({
                'Content-Type': 'application/json'
            })
        });
    }

    private handleError(error: Response) {
        //output errors to the console
        console.error(error);
        return Observable.throw(error.json().error || "Server error");
    }
}