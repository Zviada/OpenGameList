System.register(["@angular/core","@angular/router","./item.service"],function(exports_1,context_1){"use strict";var core_1,router_1,item_service_1,ItemDetailViewComponent,__decorate=this&&this.__decorate||function(decorators,target,key,desc){var d,c=arguments.length,r=c<3?target:null===desc?desc=Object.getOwnPropertyDescriptor(target,key):desc;if("object"==typeof Reflect&&"function"==typeof Reflect.decorate)r=Reflect.decorate(decorators,target,key,desc);else for(var i=decorators.length-1;i>=0;i--)(d=decorators[i])&&(r=(c<3?d(r):c>3?d(target,key,r):d(target,key))||r);return c>3&&r&&Object.defineProperty(target,key,r),r},__metadata=this&&this.__metadata||function(k,v){if("object"==typeof Reflect&&"function"==typeof Reflect.metadata)return Reflect.metadata(k,v)};context_1&&context_1.id;return{setters:[function(core_1_1){core_1=core_1_1},function(router_1_1){router_1=router_1_1},function(item_service_1_1){item_service_1=item_service_1_1}],execute:function(){ItemDetailViewComponent=function(){function ItemDetailViewComponent(itemService,router,activatedRoute){this.itemService=itemService,this.router=router,this.activatedRoute=activatedRoute}return ItemDetailViewComponent.prototype.ngOnInit=function(){var _this=this,id=+this.activatedRoute.snapshot.params.id;id?this.itemService.get(id).subscribe(function(item){return _this.item=item}):0===id?(console.log("Id is 0: switching to edit mode..."),this.router.navigate(["item/edit",0])):(console.log("Invalid id: routing back to home..."),this.router.navigate([""]))},ItemDetailViewComponent=__decorate([core_1.Component({selector:"item-detail-view",template:'\n        <div *ngIf="item" class="item-detail">\n            <h2>{{item.Title}}</h2>\n            <p>{{item.Description}}</p>\n        </div>\n    ',styles:["\n        .item-details{\n            margin: 5px;\n            padding: 5px 10px;\n            border: 1px solid black;\n            background-color: #dddddd;\n            width: 300px;\n        }\n        .item-deails * {\n            vertical-align:middle;\n        }\n        .item-detail ul li {\n            padding: 5px 0;\n        }\n    "]}),__metadata("design:paramtypes",[item_service_1.ItemService,router_1.Router,router_1.ActivatedRoute])],ItemDetailViewComponent)}(),exports_1("ItemDetailViewComponent",ItemDetailViewComponent)}}});
//# sourceMappingURL=item-detail-view.component.js.map
