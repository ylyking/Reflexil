/*
    Reflexil .NET assembly editor.
    Copyright (C) 2007 Sebastien LEBRETON

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

#region " Imports "
using System;
using System.Collections.Generic;
using System.Text;
using Reflector.CodeModel;
using Mono.Cecil;
#endregion

namespace Reflexil.Utils
{
    /// <summary>
    /// Assembly context: allow to cache methods definitions
    /// </summary>
	class AssemblyContext
    {

        #region " Fields "
        private AssemblyDefinition m_adef;
        private Dictionary<IMethodDeclaration, MethodDefinition> m_methodcache;
        #endregion

        #region " Properties "
        public AssemblyDefinition AssemblyDefinition
        {
            get
            {
                return m_adef;
            }
        }
        #endregion

        #region " Methods "
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="adef">Assembly definition</param>
        public AssemblyContext(AssemblyDefinition adef)
        {
            m_adef = adef;
            m_methodcache = new Dictionary<IMethodDeclaration, MethodDefinition>();
        }

        /// <summary>
        /// Retrieve from cache or search a method definition from Reflector's method declaration.
        /// </summary>
        /// <param name="mdec">Method declaration</param>
        /// <returns>Method definition or null if not found</returns>
        public MethodDefinition GetMethodDefinition(IMethodDeclaration mdec)
        {
            MethodDefinition result = null;

            if ((mdec != null) && (!m_methodcache.ContainsKey(mdec)))
            {
                ITypeDeclaration classdec = (ITypeDeclaration)mdec.DeclaringType;
                TypeDefinition typedef = CecilHelper.FindMatchingType(AssemblyDefinition, classdec);

                if (typedef != null)
                {
                    result = CecilHelper.FindMatchingMethod(typedef, mdec);
                    if (result != null)
                    {
                        // add result to cache
                        m_methodcache.Add(mdec, result);
                    }
                }
            }
            else
            {
                // Method definition is already cached
                result = m_methodcache[mdec];
            }

            return result;
        }
        #endregion

	}
}