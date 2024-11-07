import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { ButtonModule } from 'primeng/button';
import { FormsModule } from '@angular/forms'; 
import { CardModule } from 'primeng/card';
import { FloatLabelModule } from 'primeng/floatlabel';

@Component({
  selector: 'app-login',
  imports: [CommonModule,
    FormsModule,
    ButtonModule,
    InputTextModule,
    PasswordModule,
    CardModule,
    FloatLabelModule
  ],
  standalone: true,
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  username: string = '';
  password: string = '';

  constructor(private router: Router) { }

  login(): void {
    if (this.username === 'user' && this.password === 'password') {
      console.log('Login successful');
      this.router.navigate(['/']);
    } else {
      console.log('Login failed');
    }
  }
}
