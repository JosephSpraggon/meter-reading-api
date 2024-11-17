import { Component } from '@angular/core';
import { MeterReadingService } from '../../services/meter-reading.service'

@Component({
  selector: 'app-meter-reading-upload',
  templateUrl: './meter-reading-upload.component.html',
  styleUrls: ['./meter-reading-upload.component.css'],
})
export class MeterReadingUploadComponent {
  selectedFile: File | null = null;
  fileName: string | null = null;
  response: any;
  errorMessage: string | null = null; 

  constructor(private meterReadingService: MeterReadingService) {}

  onFileSelected(file: File | null): void {
    if (file) {
      this.selectedFile = file;
    } else {
      this.selectedFile = null;
    }
  }

  onUpload(): void {
    if (!this.selectedFile) {
      this.errorMessage = 'Please select a file before uploading.';
      return;
    }

    this.meterReadingService.uploadMeterReadings(this.selectedFile).subscribe({
      next: (response) => {
        this.errorMessage = null;
        this.response = response;
        console.log(response);
      },
      error: (err) => {
        this.response = null;
        this.errorMessage = err.error.criticalError;
        console.error(err);
      },
    });
  }
}
