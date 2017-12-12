import { Component } from "@angular/core";

@Component({
    selector: 'page-not-found',
    template: `
        <h2>{{title}}</h2>
        <div>
            Ojps.. This page does not exists (yet!).
        </div>
    `
})
export class PageNotFoundComponent {
    title = "Page not Found.";
}