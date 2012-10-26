#!/bin/bash
pushd .
LATEX=RequirementsSpecfication.tex
PDF=${LATEX/.tex/.pdf}
BASENAME=${LATEX/.tex/}
pdflatex $LATEX
makeglossaries $BASENAME
pdflatex $LATEX
open $PDF
rm ${LATEX/.tex/.aux}
rm ${LATEX/.tex/.dvi}
rm ${LATEX/.tex/.glg}
rm ${LATEX/.tex/.glo}
rm ${LATEX/.tex/.gls}
rm ${LATEX/.tex/.ist}
rm ${LATEX/.tex/.out}
rm ${LATEX/.tex/.toc}
rm ${LATEX/.tex/.log}
popd
exit 0