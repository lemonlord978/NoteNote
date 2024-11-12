import { Injectable, OnInit } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class NoteService {
  private selectedNoteId: number | null = null;

  setNoteId(noteId: number): void {
    this.selectedNoteId = noteId;
  }

  getAndClearNoteId(): number | null {
    const noteId = this.selectedNoteId;
    this.selectedNoteId = null;  
    return noteId;
  }
}
