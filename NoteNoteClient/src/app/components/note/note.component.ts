import { Component, OnInit } from '@angular/core';
import { EditorModule } from 'primeng/editor';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { FormsModule } from '@angular/forms';
import { FloatLabelModule } from 'primeng/floatlabel';
import { InputTextModule } from 'primeng/inputtext';
import { NoteApiService } from '../../services/note/note-api.service';
import { NoteService } from '../../services/note/note-service.service';
import { Router } from '@angular/router';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { RippleModule } from 'primeng/ripple';
import { Observable, Subject } from 'rxjs';
import { debounceTime, switchMap } from 'rxjs/operators';
import { ChipsModule } from 'primeng/chips';
import { Tag } from '../../models/tag';

@Component({
  selector: 'app-note',
  standalone: true,
  imports: [EditorModule,
    CommonModule,
    ButtonModule,
    FormsModule,
    FloatLabelModule,
    InputTextModule,
    ToastModule,
    RippleModule,
    ChipsModule
  ],
  templateUrl: './note.component.html',
  styleUrl: './note.component.css',
  providers: [MessageService]
})
export class NoteComponent implements OnInit {
  noteId: number | null = null;
  createUpdateNote = {
    noteId: 0,
    userId: 0,
    title: '',
    content: '',
    tags: [] as string[]
  };

  errorMessage: string = '';
  private contentChangedSubject = new Subject<void>();

  constructor(
    private apiService: NoteApiService,
    private noteService: NoteService,
    private router: Router,
    private messageService: MessageService
  ) { }

  ngOnInit(): void {
    this.clearCreateUpdateNote();

    this.noteId = this.noteService.getAndClearNoteId();

    if (this.noteId) {
      this.loadNote();
    }

    this.contentChangedSubject.pipe(
      debounceTime(1000),
      switchMap(() => this.save())
    ).subscribe();
  }

  loadNote(): void {
    if (this.noteId) {
      this.apiService.getNoteById(this.noteId).subscribe(
        (response) => {
          console.log('Note loaded successfully:', response);
          this.createUpdateNote.title = response.title;
          this.createUpdateNote.content = response.content;
          this.createUpdateNote.tags = response.tags
        },
        (error) => {
          this.errorMessage = error.error.message || 'Failed to load the note. Please try again.';
        }
      );
    }
  }

  getNewestNote(): void {
    this.createUpdateNote.userId = Number(localStorage.getItem("userId"));
    this.apiService.getNewestNote(this.createUpdateNote.userId).subscribe(
      (response) => {
        this.noteId = response.noteId;
        this.loadNote();
        console.log('Newest note fetched successfully:', response);
      },
      (error) => {
        this.errorMessage = error.error.message || 'Failed to fetch the newest note. Please try again.';
      }
    );
  }

  save(): Observable<any> {
    if (!this.noteId) {
      this.createUpdateNote.userId = Number(localStorage.getItem("userId"));
      return this.apiService.create(this.createUpdateNote).pipe(
        switchMap(() => {
          this.getNewestNote();
          console.log('Create successful');
          this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Note created successfully!' });
          return [];
        })
      );
    } else {
      this.createUpdateNote.noteId = this.noteId;
      return this.apiService.update(this.createUpdateNote).pipe(
        switchMap(() => {
          this.loadNote();
          console.log('Update successful');
          this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Note updated successfully!' });
          return [];
        })
      );
    }
  }

  onContentChange(): void {
    this.contentChangedSubject.next();
  }

  deleteNote(): void {
    if (this.noteId) {
      this.apiService.delete(this.noteId).subscribe(
        (response) => {
          this.clearCreateUpdateNote();
          console.log('Delete successful:', response);
          this.router.navigateByUrl('/');
          this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Note deleted successfully!' });
        },
        (error) => {
          console.error('Delete failed:', error);
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to delete the note.' });
        }
      );
    }
  }

  get tagNames(): string[] {
    return this.createUpdateNote.tags;
  }

  clearCreateUpdateNote() {
    this.noteId = null,
      this.createUpdateNote = {
        noteId: 0,
        userId: 0,
        title: '',
        content: '',
        tags: []
      };
  }

  generateShareLink(): string {
    if (this.noteId) {
      return `${window.location.origin}/shareNote/${this.noteId}`;
    } else {
      return '';
    }
  }

  copyToClipboard(): void {
    const link = this.generateShareLink();
    if (link) {
      const input = document.createElement('input');
      input.value = link;
      document.body.appendChild(input);
      input.select();
      document.execCommand('copy');
      document.body.removeChild(input);

      this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Link copied to clipboard!' });
    } else {
      this.messageService.add({ severity: 'error', summary: 'Error', detail: 'No valid link to copy.' });
    }
  }

}
