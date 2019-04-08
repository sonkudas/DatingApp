import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { error } from 'util';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any = { };

  constructor(public authService: AuthService, private alertifyService: AlertifyService , private router: Router) { }

  ngOnInit() {
  }
  login() {
    this.authService.login(this.model).subscribe( next => {
     // console.log ('Login Successfull');
     this.alertifyService.success('Login Successfull');
// tslint:disable-next-line: no-shadowed-variable
    }, error => {
     // console.log (error);
     this.alertifyService.error('Login Failed');
    }, () => {
      this.router.navigate(['\members']);
    });
  }

  loggedIn() {

    return this.authService.loggedIn();
  }
  logout() {
    localStorage.removeItem('token');
    this.alertifyService.message('Logged out');
    this.router.navigate(['/home']);
  }

}
