import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RedvoznjeComponent } from './redvoznje.component';

describe('RedvoznjeComponent', () => {
  let component: RedvoznjeComponent;
  let fixture: ComponentFixture<RedvoznjeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RedvoznjeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RedvoznjeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
