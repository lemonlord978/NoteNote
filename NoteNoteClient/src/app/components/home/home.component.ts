import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { LoginComponent } from '../login/login.component';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, LoginComponent, ButtonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  isLoggedIn: boolean = false;

  constructor(private router: Router) {}

  ngOnInit(): void {
    this.isLoggedIn = false; 
  }

  login(): void {
    this.router.navigate(['/login']);
  }
}
