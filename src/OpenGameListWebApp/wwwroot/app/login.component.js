System.register(["@angular/core","@angular/forms","@angular/router","./auth.service"],function(exports_1,context_1){"use strict";var core_1,forms_1,router_1,auth_service_1,LoginComponent,__decorate=this&&this.__decorate||function(decorators,target,key,desc){var d,c=arguments.length,r=c<3?target:null===desc?desc=Object.getOwnPropertyDescriptor(target,key):desc;if("object"==typeof Reflect&&"function"==typeof Reflect.decorate)r=Reflect.decorate(decorators,target,key,desc);else for(var i=decorators.length-1;i>=0;i--)(d=decorators[i])&&(r=(c<3?d(r):c>3?d(target,key,r):d(target,key))||r);return c>3&&r&&Object.defineProperty(target,key,r),r},__metadata=this&&this.__metadata||function(k,v){if("object"==typeof Reflect&&"function"==typeof Reflect.metadata)return Reflect.metadata(k,v)};context_1&&context_1.id;return{setters:[function(core_1_1){core_1=core_1_1},function(forms_1_1){forms_1=forms_1_1},function(router_1_1){router_1=router_1_1},function(auth_service_1_1){auth_service_1=auth_service_1_1}],execute:function(){LoginComponent=function(){function LoginComponent(fb,router,authService){this.fb=fb,this.router=router,this.authService=authService,this.title="Login",this.loginForm=null,this.loginError=!1,this.loginForm=fb.group({username:["",forms_1.Validators.required],password:["",forms_1.Validators.required]})}return LoginComponent.prototype.performLogin=function(e){var _this=this;e.preventDefault();var username=this.loginForm.value.username,password=this.loginForm.value.password;this.authService.login(username,password).subscribe(function(data){_this.loginError=!1;var auth=_this.authService.getAuth();alert("Our Token is: "+auth.access_token),_this.router.navigate([""])},function(err){console.log(err),_this.loginError=!0})},LoginComponent=__decorate([core_1.Component({selector:"login",template:'\n<div class="login-container">\n    <h2 class="form-login-heading">Login</h2>\n    <div class="alert alert-danger" role="alert" *ngIf="loginError">\n        <strong>Warning:</strong> Username or Password mismatch\n    </div>\n    <form class="form-login" [formGroup]="loginForm" (submit)="performLogin($event)">\n        <input formControlName="username" type="text" class="form-control" placeholder="Your username or e-mail address" required autofocus />\n        <input formControlName="password" type="password" class="form-control" placeholder="Your password" required />\n        <div class="checkbox">\n            <label><input type="checkbox" value="remember-me"/>Remember me</label>\n        </div>\n        <button class="btn btn-lg btn-primary btn-block" type="submit">Sign in</button>\n    </form>\n</div>\n    '}),__metadata("design:paramtypes",[forms_1.FormBuilder,router_1.Router,auth_service_1.AuthService])],LoginComponent)}(),exports_1("LoginComponent",LoginComponent)}}});
//# sourceMappingURL=login.component.js.map
