import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { MenuItem } from 'primeng/api';
import { MenubarModule } from 'primeng/menubar';
import { MenuModule } from 'primeng/menu';
import { BadgeModule } from 'primeng/badge';
import { AvatarModule } from 'primeng/avatar';
import { InputTextModule } from 'primeng/inputtext';
import { CommonModule } from '@angular/common';
import { RippleModule } from 'primeng/ripple';
import { ButtonModule } from 'primeng/button';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { UserApiService } from './services/user/user-api.service';
import { DropdownModule } from 'primeng/dropdown';
import { SearchService } from './services/search/search.service';
import { FormsModule } from '@angular/forms';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet,
    MenubarModule,
    BadgeModule,
    AvatarModule,
    InputTextModule,
    RippleModule,
    CommonModule,
    ButtonModule,
    HomeComponent,
    LoginComponent,
    DropdownModule,
    MenuModule,
    FormsModule
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  searchQuery: string = '';
  private searchSubject = new Subject<string>();
  private searchSubscription;

  items: MenuItem[] | undefined;
  menuItems: MenuItem[] | undefined;
  isLoggedIn: boolean = false;

  username: string = '';

  constructor(private router: Router,
    private apiService: UserApiService,
    private cdRef: ChangeDetectorRef,
    private searchService: SearchService
  ) {
    this.searchSubscription = this.searchSubject.pipe(
      debounceTime(500)
    ).subscribe(query => {
      this.searchService.updateSearchQuery(query);
    });

  }

  ngOnInit(): void {
    this.items = [
      {
        label: 'Home',
        icon: 'pi pi-home',
        command: () => {
          this.router.navigate(['/']);
        }
      },
      {
        label: 'Contact',
        icon: 'pi pi-envelope'
      }
    ];
    this.menuItems = [
      {
        label: 'Profile',
        icon: 'pi pi-user',
        command: () => this.viewProfile()
      },
      {
        label: 'Logout',
        icon: 'pi pi-sign-out',
        command: () => this.logout()
      }
    ];
    this.checkLoginStatus();
  }

  checkLoginStatus(): void {
    const userId = typeof window !== 'undefined' ? Number(localStorage.getItem('userId')) : null;
    if (userId) {
      this.isLoggedIn = true;
      this.fetchUserDetails(Number(userId));
    }
  }

  fetchUserDetails(userId: number): void {
    this.apiService.getUserById(userId).subscribe(
      (user) => {
        this.username = user.username;
        this.isLoggedIn = true;
        this.cdRef.detectChanges();
      },
      (error) => {
        console.error('Error fetching user details:', error);
        this.logout();
      }
    );
  }

  viewProfile(): void {
    this.router.navigate(['/profile']);
  }

  login(): void {
    this.router.navigate(['/login']).then(() => {
      this.checkLoginStatus();
    });
  }

  logout(): void {
    localStorage.removeItem('userId');
    this.isLoggedIn = false;
    this.username = '';
    this.cdRef.detectChanges();
    this.router.navigateByUrl('/').then(() => {
      location.reload();
    });
  }

  onSelect(event: any): void {
    console.log('Selected:', event.value);
    if (event.value.label === 'Profile') {
      this.viewProfile();
    } else if (event.value.label === 'Logout') {
      this.logout();
    }
  }

  onSearchChange(query: string) {
    this.searchService.updateSearchQuery(query);
  }
}
