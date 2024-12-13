import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NoteApiService } from '../../../../services/note/note-api.service';
import { FormsModule } from '@angular/forms';
import { ChipsModule } from 'primeng/chips';
import { ToastModule } from 'primeng/toast';
import { EditorModule } from 'primeng/editor';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-share-note',
  standalone: true,
  imports: [
    FormsModule,
    ChipsModule,
    ToastModule,
    EditorModule
  ],
  templateUrl: './share-note.component.html',
  styleUrls: ['./share-note.component.css'],
  providers: [MessageService]
})
export class ShareNoteComponent implements OnInit {
  noteId: number | null = null;
  createUpdateNote = {
    noteId: 0,
    userId: 0,
    title: '',
    content: '',
    tags: [] as string[]
  };
  errorMessage: string = '';

  constructor(
    private apiService: NoteApiService,
    private route: ActivatedRoute,
    private messageService: MessageService
  ) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const noteIdFromRoute = params.get('noteId');
      if (noteIdFromRoute) {
        this.noteId = +noteIdFromRoute;
        this.loadNote();
      } else {
        this.errorMessage = 'Invalid note ID.';
      }
    });
  }

  loadNote(): void {
    if (this.noteId !== null) {
      this.apiService.shareNote(this.noteId).subscribe(
        (response) => {
          console.log('Note loaded successfully:', response);
          this.createUpdateNote.title = response.title;
          this.createUpdateNote.content = response.content;
          this.createUpdateNote.tags = response.tags;
        },
        (error) => {
          this.errorMessage = error.error.message || 'Failed to load the note. Please try again.';
        }
      );
    } else {
      this.errorMessage = 'Note ID is missing.';
    }
  }
}
