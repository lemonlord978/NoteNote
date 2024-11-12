import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { ButtonModule } from 'primeng/button';
import { FormsModule } from '@angular/forms'; 
import { CardModule } from 'primeng/card';
import { FloatLabelModule } from 'primeng/floatlabel';
import { UserApiService } from '../../services/user/user-api.service';
import { SelectButtonModule } from 'primeng/selectbutton';

@Component({
  selector: 'app-login',
  imports: [CommonModule,
    FormsModule,
    ButtonModule,
    InputTextModule,
    PasswordModule,
    CardModule,
    FloatLabelModule,
    SelectButtonModule
  ],
  standalone: true,
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  stateOptions: any[] = [{ label: 'Login', value: 'Login' },{ label: 'Register', value: 'Register' }];

  value: string = 'Login';

  username: string = '';
  password: string = '';
  email: string = '';
  errorMessage: string = '';

  constructor(private apiService: UserApiService, private router: Router) { }

  login(): void {
    this.apiService.login(this.username, this.password).subscribe(
      (response) => {
        console.log('Login successful:', response);
        
        localStorage.setItem('userId', response.userId.toString());
        
        this.router.navigateByUrl('/').then(()=>{
          location.reload();
        });
      },
      (error) => {
        this.errorMessage = error.error.message || 'Login failed, please try again.';
      }
    );
  }

  register(): void {
    this.apiService.register(this.username, this.password, this.email).subscribe(
      (response) => {
        console.log('Register successful:', response);
        
        localStorage.setItem('userId', response.userId.toString());
        
        this.router.navigateByUrl('/').then(()=>{
          location.reload();
        });
      },
      (error) => {
        this.errorMessage = error.error.message || 'Register failed, please try again.';
      }
    );
  }
}
