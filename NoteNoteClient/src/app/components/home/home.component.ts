import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { LoginComponent } from '../login/login.component';
import { ButtonModule } from 'primeng/button';
import { UserApiService } from '../../services/user/user-api.service';
import { CardModule } from 'primeng/card';
import { NoteApiService } from '../../services/note/note-api.service';
import { DatePipe } from '@angular/common';
import { NoteService } from '../../services/note/note-service.service';
import { TagModule } from 'primeng/tag';
import { PaginatorModule } from 'primeng/paginator';
import { Subscription } from 'rxjs';
import { SearchService } from '../../services/search/search.service';
import { DropdownModule } from 'primeng/dropdown';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule,
    LoginComponent,
    ButtonModule,
    TagModule,
    CardModule,
    PaginatorModule,
    DropdownModule
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
  providers: [DatePipe]
})
export class HomeComponent implements OnInit {
  searchQuery = '';
  private searchSubscription!: Subscription;

  currentPage: number = 1;
  pageSize: number = 10;
  totalItems: number = 0;

  notes: any[] = [];

  isLoggedIn: boolean = false;
  username: string = '';

  showNotFound: boolean = false;

  selectedOrder: string = "Title asc";

  orderOptions = [
    { label: 'Order By Asc', value: 'Title asc' },
    { label: 'Order By Desc', value: 'Title desc' }
  ];

  constructor(private router: Router,
    private datePipe: DatePipe,
    private apiService: UserApiService,
    private noteApiService: NoteApiService,
    private noteService: NoteService,
    private searchService: SearchService
  ) { }

  ngOnInit(): void {
    this.isLoggedIn = false;
    this.checkLoginStatus();
    this.searchSubscription = this.searchService.searchQuery$.subscribe(
      (query) => {
        this.searchQuery = query;
      }
    );
  }

  checkLoginStatus(): void {
    const userId = typeof window !== 'undefined' ? Number(localStorage.getItem('userId')) : null;
    if (userId) {
      this.isLoggedIn = true;
    }
    this.searchSubscription = this.searchService.searchQuery$.subscribe(
      (query) => {
        this.searchQuery = query;
        this.getNote();
      }
    );
  }

  ngOnDestroy(): void {
    if (this.searchSubscription) {
      this.searchSubscription.unsubscribe();
    }
  }

  login(): void {
    this.router.navigate(['/login']);
  }

  create(): void {
    this.router.navigate(['/note']);
  }

  getNote(): void {
    const userId = typeof window !== 'undefined' ? Number(localStorage.getItem('userId')) : null;
    if (userId) {

      this.noteApiService.getAllNotes(userId, this.searchQuery, this.selectedOrder).subscribe(
        (response) => {
          this.showNotFound = false; 
          this.notes = response.map((note: any) => ({
            ...note,
            updatedAtFormatted: this.formatDate(note.updatedAt),
          }));
          console.log('Notes retrieved successfully:', this.notes);
        },
        (error) => {
          if (error.status === 404) {
            this.showNotFound = true; 
          } else {
            console.error('Error retrieving notes:', error);
          }
        }
      );
    }
  }

  onOrderChange(event: any): void {
    this.selectedOrder = event.value;
    console.log('Order changed to:', event);
    this.getNote();
  }

  formatDate(dateString: string): string | null {
    if (!dateString) {
      return null;
    }

    const date = new Date(dateString);

    if (isNaN(date.getTime())) {
      console.error('Invalid date format:', dateString);
      return null;
    }

    return this.datePipe.transform(date, 'MM/dd/yyyy hh:mm:ss a');
  }

  viewNote(noteId: number): void {
    this.noteService.setNoteId(noteId);
    this.router.navigate(['/note']);
  }

  deleteNote(noteId: number): void {
    this.noteApiService.delete(noteId).subscribe(
      (response) => {
        console.log('Notes retrieved successfully:');
        location.reload();
      },
      (error) => {
        console.error('Error retrieving notes');
      }
    );
  }
  prevPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.getNote();
    }
  }

  nextPage(): void {
    if (this.currentPage < Math.ceil(this.totalItems / this.pageSize)) {
      this.currentPage++;
      this.getNote();
    }
  }
}
