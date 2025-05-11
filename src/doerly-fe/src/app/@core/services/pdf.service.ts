import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as pdfjsLib from 'pdfjs-dist';
import { PDFDocumentProxy } from 'pdfjs-dist';
import { environment } from 'environments/environment.development';
import { from, throwError } from 'rxjs';
import { map, switchMap, catchError } from 'rxjs/operators';
import { firstValueFrom } from 'rxjs';
import { ErrorMessageCode } from '../constants/error-messages';

@Injectable({
  providedIn: 'root'
})
export class PdfService {
  private pdfDoc: PDFDocumentProxy | null = null;
  private currentPage = 1;
  private pageCount = 0;

  constructor(
    private httpClient: HttpClient
  ) {
    // Initialize PDF.js worker
    pdfjsLib.GlobalWorkerOptions.workerSrc = 'assets/pdf.worker.mjs';
  }

  loadPdf(url: string): Promise<void> {
    const proxyUrl = url.replace(environment.azureBlobStorageUrl, '');

    return firstValueFrom(
      this.httpClient.get(proxyUrl, { responseType: 'blob' })
        .pipe(
          switchMap(pdfBlob => {
            const blobUrl = URL.createObjectURL(pdfBlob);
            return from(pdfjsLib.getDocument(blobUrl).promise)
              .pipe(
                map(pdf => {
                  this.pdfDoc = pdf;
                  this.pageCount = pdf.numPages;
                  this.currentPage = 1;
                  URL.revokeObjectURL(blobUrl);
                }),
                catchError(_ => {
                  URL.revokeObjectURL(blobUrl);
                  return throwError(() => ErrorMessageCode.PDF_LOAD_ERROR);
                })
              );
          }),
          catchError(_ => throwError(() => ErrorMessageCode.PDF_LOAD_ERROR))
        )
    );
  }

  renderPage(pageNumber: number, container: HTMLElement): Promise<void> {
    if (!this.pdfDoc) {
      return Promise.reject(ErrorMessageCode.PDF_NO_DOCUMENT);
    }

    return firstValueFrom(
      from(this.pdfDoc.getPage(pageNumber))
        .pipe(
          switchMap(page => {
            const viewport = page.getViewport({ scale: 1.5 });
            const canvas = document.createElement('canvas');
            const context = canvas.getContext('2d');

            if (!context) {
              return throwError(() => ErrorMessageCode.PDF_CANVAS_CONTEXT);
            }

            canvas.height = viewport.height;
            canvas.width = viewport.width;

            // Clear previous content
            container.innerHTML = '';
            container.appendChild(canvas);

            return from(page.render({
              canvasContext: context,
              viewport: viewport
            }).promise);
          }),
          catchError(_ => throwError(() => ErrorMessageCode.PDF_RENDER_ERROR))
        )
    );
  }

  getCurrentPage(): number {
    return this.currentPage;
  }

  getPageCount(): number {
    return this.pageCount;
  }

  nextPage(container: HTMLElement): Promise<void> {
    if (this.currentPage < this.pageCount) {
      this.currentPage++;
      return this.renderPage(this.currentPage, container);
    }
    return Promise.resolve();
  }

  previousPage(container: HTMLElement): Promise<void> {
    if (this.currentPage > 1) {
      this.currentPage--;
      return this.renderPage(this.currentPage, container);
    }
    return Promise.resolve();
  }

  reset(): void {
    this.pdfDoc = null;
    this.currentPage = 1;
    this.pageCount = 0;
  }
}
